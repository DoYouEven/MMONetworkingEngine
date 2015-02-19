public enum PacketTypes   //Header -> Header2
{
    None,
    Special , //chat
    Game,  //loginRequest
    Exception,  //loginREsponse
    Forward,
    Uint,
    Byte,
    UShort
}
public enum SpecialRequest  //Header -> Header2
{
    None,
    LoginRequest, //chat
    SendChat,  //loginREsponse
   
    Uint,
    Byte,
    UShort
}
public enum SpecialResponse  //Header -> Header2
{
    ConnectionResponse,
    LoginResponse,  //loginRequest
    RecieveChat,
     PlayerLoginEvent = 0x7

}
public enum LoginResponseEC
{
    None,
    LoginSuccess,
    LoginFailure
}
public enum GameHeader  //Header -> Header2
{
    None,
    Movement,//chat
    Game,  //loginRequest
    Exception,  //loginREsponse
    Forward,
    Uint,
    Byte,
    UShort
}
// NEed to decide whether we want to have explicit headers for obvious packets like chat/login.
//Login seems to be a fairly overused method that every MMO has, and so does chat

//Implicit messaging, is where we give a broad message type.eg. Login, broadcast all, broadcast single, to server only

//Forward to player ID
//Chat Packet
//Broadcast
//To serverOnly
//Server Admin
//RFC
// broadcas--> RFC ID , objec params[])
// ALl RFC ids are scanned and added to list before game starts
//methods which have a tag [RFC] are added to hastable per byte
//Sice every version of the client has the same methods, the byte to method correaltion will always be the same


//dicitonar =    for every byte(rfcid) there is one mapped method
// on receive-> table[rfcid].Invoke(params[]);
//chat( stirng chat stuff)
//RFC all methods from the game are bound to RFC  method = examplerfc( RFC ID, object params[])
//dictionary  . Map[rfcid] = method;
//Map[rfcid].invoke();


public enum PrefixTypes
{
    Bool = 1,
    Int = 2,
    String = 3,
    Float = 4,
    Uint = 5,
    Byte = 6,
    UShort = 7,
    Int8 = 108,  //(user wants int, but lenght is only <8 bits)
    Int16 = 109
}