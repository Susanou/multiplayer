using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CsServer
{
    public class InputManager
    {
        public enum Keys
        {
            None,
            W,
            A,
            S,
            D

        }

        public static void TryToMove(int connectionID, Keys key)
        {
            Vector3 tmpPosition = GameManager.playerList[connectionID].position;

            if (key == Keys.None) return;

            Player player = GameManager.playerList[connectionID];

            if(key == Keys.W)
            {
                tmpPosition.X += GameManager.playerSpeed;
                tmpPosition.Z += GameManager.playerSpeed;
            }
            else if(key == Keys.S)
            {
                tmpPosition.X -= GameManager.playerSpeed;
                tmpPosition.Z -= GameManager.playerSpeed;
            }
            else if(key == Keys.A)
            {
                tmpPosition.X -= GameManager.playerSpeed;
                tmpPosition.Z += GameManager.playerSpeed;
            }
            else if(key == Keys.D)
            {
                tmpPosition.X += GameManager.playerSpeed;
                tmpPosition.Z -= GameManager.playerSpeed;
            }

            //player.position = tmpPosition;
            GameManager.playerList[connectionID].position = tmpPosition;

            NetworkSend.SendPlayerMove(connectionID, tmpPosition.X, tmpPosition.Y, tmpPosition.Z);
        }

    }
}
