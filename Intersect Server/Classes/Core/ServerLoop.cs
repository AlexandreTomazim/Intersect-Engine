﻿/*
    Intersect Game Engine (Server)
    Copyright (C) 2015  JC Snider, Joe Bridges
    
    Website: http://ascensiongamedev.com
    Contact Email: admin@ascensiongamedev.com 

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System;
using System.Threading;
using Intersect_Server.Classes.General;
using Intersect_Server.Classes.Maps;
using Intersect_Server.Classes.Networking;

namespace Intersect_Server.Classes.Core
{
    public static class ServerLoop
    {
        public static void RunServerLoop()
        {
            long cpsTimer = Globals.System.GetTimeMs() + 1000;
            long cps = 0;
            while (Globals.ServerStarted)
            {
                foreach (var map in MapInstance.GetObjects())
                {
                    if (map.Value.Active) map.Value.Update();
                }
                cps++;
                if (Globals.System.GetTimeMs() >= cpsTimer)
                {
                    Globals.CPS = cps;
                    cps = 0;
                    cpsTimer = Globals.System.GetTimeMs() + 1000;
                }
                if (Globals.CPSLock) { Thread.Sleep(10); }
            }

            //Server is shutting down!!
            //TODO gracefully disconnect all clients
            SocketServer.Stop();
            WebSocketServer.Stop();
        }
    }
}
