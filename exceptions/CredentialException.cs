using System;

namespace ZoteroApiClientCs.Api.exceptions
{
    public class CredentialException: Exception
    {
        public CredentialException(string message): base(message)
        {
            
        }
        
        public CredentialException(string message, Exception inner): base(message, inner)
        {
            
        }
        
    }
}