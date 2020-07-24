using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace KeybordFunctions
{
    public static class kfunctions
    {
        public static char getnextkey()
        {
            // Read keyboard
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.A))
                return 'A';
            else if (keys.IsKeyDown(Keys.B))
                return 'B';
            else if (keys.IsKeyDown(Keys.C))
                return 'C';
            else if (keys.IsKeyDown(Keys.D))
                return 'D';
            else if (keys.IsKeyDown(Keys.E))
                return 'E';
            else if (keys.IsKeyDown(Keys.F))
                return 'F';
            else if (keys.IsKeyDown(Keys.G))
                return 'G';
            else if (keys.IsKeyDown(Keys.H))
                return 'H';
            else if (keys.IsKeyDown(Keys.I))
                return 'I';
            else if (keys.IsKeyDown(Keys.J))
                return 'J';
            else if (keys.IsKeyDown(Keys.K))
                return 'K';
            else if (keys.IsKeyDown(Keys.L))
                return 'L';
            else if (keys.IsKeyDown(Keys.M))
                return 'M';
            else if (keys.IsKeyDown(Keys.N))
                return 'N';
            else if (keys.IsKeyDown(Keys.O))
                return 'O';
            else if (keys.IsKeyDown(Keys.P))
                return 'P';
            else if (keys.IsKeyDown(Keys.Q))
                return 'Q';
            else if (keys.IsKeyDown(Keys.R))
                return 'R';
            else if (keys.IsKeyDown(Keys.S))
                return 'S';
            else if (keys.IsKeyDown(Keys.T))
                return 'T';
            else if (keys.IsKeyDown(Keys.U))
                return 'U';
            else if (keys.IsKeyDown(Keys.V))
                return 'V';
            else if (keys.IsKeyDown(Keys.W))
                return 'W';
            else if (keys.IsKeyDown(Keys.X))
                return 'X';
            else if (keys.IsKeyDown(Keys.Y))
                return 'Y';
            else if (keys.IsKeyDown(Keys.Z))
                return 'Z';
            else if (keys.IsKeyDown(Keys.D0))
                return '0';
            else if (keys.IsKeyDown(Keys.D1))
                return '1';
            else if (keys.IsKeyDown(Keys.D2))
                return '2';
            else if (keys.IsKeyDown(Keys.D3))
                return '3';
            else if (keys.IsKeyDown(Keys.D4))
                return '4';
            else if (keys.IsKeyDown(Keys.D5))
                return '5';
            else if (keys.IsKeyDown(Keys.D6))
                return '6';
            else if (keys.IsKeyDown(Keys.D7))
                return '7';
            else if (keys.IsKeyDown(Keys.D8))
                return '8';
            else if (keys.IsKeyDown(Keys.D9))
                return '9';
            else if (keys.IsKeyDown(Keys.Space))
                return ' ';
            else
                return '!';
        }
    }
}
