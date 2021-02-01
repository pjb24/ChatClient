using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace TestClient
{
    static class Initializer
    {
        public static string user_ID = null;

        public static List<string> userList = new List<string>();
        public static List<string> groupList = new List<string>();

        public static TcpClient clientSocket = new TcpClient();
        public static NetworkStream stream = default(NetworkStream);

        public static TestClientUI testClientUI = new TestClientUI();
        public static SignInForm signInForm = new SignInForm();
        public static GroupForm groupForm = new GroupForm();
        public static List<ChatGroupForm> chatGroupForms = new List<ChatGroupForm>();
    }
}
