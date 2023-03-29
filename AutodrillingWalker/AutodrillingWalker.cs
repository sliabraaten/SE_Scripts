﻿using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        IMyPistonBase leftPistonX, leftPistonY, rightPistonX, rightPistonY;
        IMyLandingGear rightMagPlate, leftMagPlate;
        public Program()
        {
            //AutoUpdate
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            leftPistonX = GridTerminalSystem.GetBlockWithName("Piston X Left") as IMyPistonBase;
            leftPistonY = GridTerminalSystem.GetBlockWithName("Piston Y Left") as IMyPistonBase;
            rightPistonX = GridTerminalSystem.GetBlockWithName("Piston X Right") as IMyPistonBase;
            rightPistonY = GridTerminalSystem.GetBlockWithName("Piston Y Right") as IMyPistonBase;
            rightMagPlate = GridTerminalSystem.GetBlockWithName("Mag Plate Right") as IMyLandingGear;
            leftMagPlate = GridTerminalSystem.GetBlockWithName("Mag Plate Left") as IMyLandingGear;


        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (leftPistonX.CurrentPosition == leftPistonX.MaxLimit)
            {
                rightMagPlate.Lock();
                if (rightMagPlate.IsLocked)
                {
                    leftMagPlate.Unlock();
                    leftPistonY.Retract();
                    leftPistonX.Retract();
                    rightPistonX.Extend();
                }

            }
            else if (leftPistonX.CurrentPosition == leftPistonX.MinLimit)
            {
                leftPistonY.Extend();
                leftMagPlate.Lock();
                if (leftMagPlate.IsLocked)
                {
                    rightMagPlate.Unlock();
                    rightPistonY.Retract();
                    leftPistonX.Extend();
                    rightPistonX.Retract();
                }
                
            }
        }
    //end class
    
    }
}
