using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace TestClient
{
    static class GlobalClass
    {
        public static SignInForm signInForm = new SignInForm();
        public static LobbyForm lobbyForm = new LobbyForm();
        public static RegisterForm registerForm = new RegisterForm();
    }
}
