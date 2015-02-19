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
    int mSize = 0;
    bool isWriting = false;
    bool isAvaliable= false;
    public PacketBuffer()
    {
        _memStream = new MemoryStream();
        _binWriter = new BinaryWriter(_memStream);
        _binReader = new BinaryReader(_memStream);
    }
    public int position { get { return (int)_memStream.Position; } set { _memStream.Seek(value, SeekOrigin.Begin); } }
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
    /// <summary>
    /// Init Packet
    /// </summary>

    static public PacketBuffer CreatePacket(PacketTypes packet, int startOffset = 0)
    {
        PacketBuffer buffer = Create(true);
        BinaryWriter writer = buffer.BeginWriting(startOffset);
        writer.Write(0);
        writer.Write((byte)packet);
        return buffer;
    }
    public BinaryWriter BeginWriting(int startOffset)
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
    public BinaryWriter BeginWriting(bool append)
    {
        if (!append || !isWriting)
        {
            _memStream.Seek(0, SeekOrigin.Begin);
            mSize = 0;
        }
        isWriting = true;
        return _binWriter;
    }

    public int EndPacket()
    {
        if (isWriting)
        {
            mSize = position;
            _memStream.Seek(0, SeekOrigin.Begin);
            _binWriter.Write(mSize - 4);
            _memStream.Seek(0, SeekOrigin.Begin);
            isWriting = false;
        }
        return mSize;
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
    public int PeekInt(int offset)
    {
        long pos = _memStream.Position;
        if (offset + 4 > pos) return -1;
        _memStream.Seek(offset, SeekOrigin.Begin);
        int val = _binReader.ReadInt16();
        //_memStream.Seek(pos, SeekOrigin.Begin);
        return val;
    }
    public float PeekFloat(int offset)
    {
        long pos = _memStream.Position;
        if (offset + 4 > pos) return -1;
        _memStream.Seek(offset, SeekOrigin.Begin);
        float val = _binReader.ReadInt16();
        //_memStream.Seek(pos, SeekOrigin.Begin);
        return val;
    }
}
