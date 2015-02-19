using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;




public class PacketBuffer
{
   

    static Queue<PacketBuffer> _bufferPool = new Queue<PacketBuffer>();
    private MemoryStream _memStream;
    private BinaryWriter _binWriter;
    private BinaryReader _binReader;
    public int _size = 0;
    bool isWriting = false;
    bool isAvaliable= false;
    public PacketBuffer()   //Disks
    {
        _memStream = new MemoryStream();  //Track with spinning arm
        _binWriter = new BinaryWriter(_memStream);  //Printing on disk
        _binReader = new BinaryReader(_memStream);  //scanning
    }
    public int position { get { return (int)_memStream.Position; } set { _memStream.Seek(value, SeekOrigin.Begin); } }
    public int size
    {
        get
        {
            return isWriting ? (int)_memStream.Position : _size - (int)_memStream.Position;
        }
    }
    public const int BUFFER_SIZE = 1024;
    public Socket workSocket = null;
    public MemoryStream stream
    {
        get { return _memStream; }
    }
    public byte[] buffer
    {
        get { return _memStream.GetBuffer(); }
    }
    //public byte[] buffer = new byte[BUFFER_SIZE];

    public StringBuilder sb = new StringBuilder();

    public int dataRecieved = 0;
    public int dataSize = 0;

    /// <summary>
    /// Initialize our current PacketBuffer
    /// </summary>
    /// <returns></returns>
    static public PacketBuffer Init()
    {
        PacketBuffer buffer = new PacketBuffer();
        return buffer;
    }
    static public PacketBuffer Create(bool markAsUsed)
	{
        PacketBuffer buffer = null;

        if (_bufferPool.Count == 0)
		{
            buffer = new PacketBuffer();
		}
		else
		{
            lock (_bufferPool)
			{
                if (_bufferPool.Count != 0)
				{
                    buffer = _bufferPool.Dequeue();
                    buffer.isAvaliable = false;
				}
                else buffer = new PacketBuffer();
			}
		}
		//b.mCounter = markAsUsed ? 1 : 0;
		return buffer;
	}

    public void AddToFreeList()
    {

            lock (_bufferPool)
            {
                ResetBuffer();
                _bufferPool.Enqueue(this);
            }
 

    }
    public void ResetBuffer()
    {
       // mCounter = 0;
        _size = 0;
        _memStream.Seek(0, SeekOrigin.Begin);
    }
    /// <summary>
    /// Attempt to create a packet from the queue 
    /// Then we write the lenght(0 for now). BAsically making room for it .
    /// Then we write the byte of the header
    /// </summary>
    /// <param name="packet"></param>
    /// <param name="startOffset"></param>
    /// <returns></returns>
    static public PacketBuffer CreatePacket(PacketTypes packet, int startOffset = 0)
    {
        PacketBuffer buffer = Create(true);
        BinaryWriter writer = buffer.StartWriting(startOffset);
        writer.Write(0);
        writer.Write((byte)packet);
        return buffer;
    }

    static public PacketBuffer Initialize()
    {
        PacketBuffer buffer = Create(true);
        return buffer;
    }
    public BinaryWriter StartWriting(int startOffset = 0)
    {
        _memStream.Seek(startOffset, SeekOrigin.Begin);
        isWriting = true;
        return _binWriter;
    }
    /// <summary>
    /// Basically intiate the packet
    /// </summary>
    /// <param name="append"></param>
    /// <returns></returns>
    public BinaryWriter StartWriting(bool append)
    {
        if (!append || !isWriting)
        {
            _memStream.Seek(0, SeekOrigin.Begin);
            _size = 0;
        }
        isWriting = true;
        return _binWriter;
    }


    /// <summary>
    /// End the packet with lenght, we are moving to the front of the packet where we had previously reserved 4 bytes to store this.
    /// </summary>
    /// <returns></returns>
    ///    
    /// 
   
    public int EndPacket()
    {
    
            _size = position;
            _memStream.Seek(0, SeekOrigin.Begin);  //Needs to go back to star to write hte lenght
            _binWriter.Write(_size - 4);  //we have moved 4(lenght) + lenght of packet
            _memStream.Seek(0, SeekOrigin.Begin);
            isWriting = false;
       
        return _size;
    }
    public int PeekByte(int offset)
    {
        long pos = _memStream.Position;
        if (offset + 1 > pos) return -1;
        _memStream.Seek(offset, SeekOrigin.Begin);
        int val = _memStream.ReadByte();
        _memStream.Seek(pos, SeekOrigin.Begin);
        return val;
    }
    public byte[] ReadBytes(int count)
    {
        return _binReader.ReadBytes(count);
    }
    public int  PeekInt(int offset)
    {
        long pos = _memStream.Position;
        if (offset + 4 > pos)
        {
            return -1;
        }
        _memStream.Seek(offset, SeekOrigin.Begin);
        int val = _binReader.ReadInt32();
        _memStream.Seek(pos, SeekOrigin.Begin);
        return val;
    }


    /// <summary>
    /// Read a float
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public float PeekFloat(int offset)
    {
        long pos = _memStream.Position;
       // if (offset + 4 > pos) return -1;
        _memStream.Seek(offset, SeekOrigin.Begin);
        float val = _binReader.ReadSingle();
        //_memStream.Seek(pos, SeekOrigin.Start);
        return val;
    }

    /// <summary>
    /// Enable the reading process
    /// </summary>
    /// <returns></returns>
    /// 
    public BinaryReader ContinueReading()
    {
        return _binReader;
    }
    public BinaryReader StartReading()
    {
        if (isWriting)
        {
            isWriting = false;
            _size = position;
            _memStream.Seek(0, SeekOrigin.Begin);
        }
        return _binReader;
    }
    public BinaryReader StartReading(int startOffset)
    {
        if (isWriting)
        {
            isWriting = false;
            _size = (int)_memStream.Position;
        }
        _memStream.Seek(startOffset, SeekOrigin.Begin);
        return _binReader;
    }

}
