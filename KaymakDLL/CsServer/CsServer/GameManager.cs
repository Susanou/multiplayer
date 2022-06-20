using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsServer
{
    static class GameManager
    {

        public static Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        public static float playerSpeed = 0.01f;

        public static void JoinGame(int connectionID, Player player)
        {
            NetworkSend.InstantiateNetworkPlayer(connectionID, player);
        }

        public static void createPlayer(int connectionID)
        {
            Player player = new Player()
            {
                connectionID = connectionID,
                inGame = true
            };

            playerList.Add(connectionID, player);
            Console.WriteLine("Player {0} has been added to the game", connectionID);
            JoinGame(connectionID, player);
        }
    }

}
