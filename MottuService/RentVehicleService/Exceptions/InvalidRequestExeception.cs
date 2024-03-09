public class InvalidRequestExeception : Exception
{
    public InvalidRequestExeception() : base(){  }
    public InvalidRequestExeception(string message) : base(message){   }
}
