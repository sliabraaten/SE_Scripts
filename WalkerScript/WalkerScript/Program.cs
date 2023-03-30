using Sandbox.Game.EntityComponents;
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
        IMyExtendedPistonBase leftPistonX, leftPistonY, rightPistonX, rightPistonY;
        IMyLandingGear rightMagPlate, leftMagPlate;
        bool leftLegActive, legLock;

        public Program()
        {
            //AutoUpdate
            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            leftPistonX = GridTerminalSystem.GetBlockWithName("Piston X Left") as IMyExtendedPistonBase;
            leftPistonY = GridTerminalSystem.GetBlockWithName("Piston Y Left") as IMyExtendedPistonBase;
            rightPistonX = GridTerminalSystem.GetBlockWithName("Piston X Right") as IMyExtendedPistonBase;
            rightPistonY = GridTerminalSystem.GetBlockWithName("Piston Y Right") as IMyExtendedPistonBase;
            rightMagPlate = GridTerminalSystem.GetBlockWithName("Mag Plate Right") as IMyLandingGear;
            leftMagPlate = GridTerminalSystem.GetBlockWithName("Mag Plate Left") as IMyLandingGear;


            if (rightMagPlate.IsLocked && leftMagPlate.IsLocked)
                rightMagPlate.Unlock();

            LegInit(leftPistonX, leftPistonY, leftMagPlate);
            LegInit(rightPistonX, rightPistonY, rightMagPlate);

            leftLegActive = true;
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {


            if (leftLegActive)
            {
                //todo handle leg not locking and error 
                legLock = StepLeg(leftPistonX, leftPistonY, leftMagPlate, rightMagPlate);
                if(legLock)
                    ResetLeg(rightPistonX, rightPistonY);
            }
            else
            {
                legLock = StepLeg(rightPistonX, rightPistonY, rightMagPlate, leftMagPlate);
                if(legLock)
                    ResetLeg(leftPistonX, leftPistonY);
            }
            leftLegActive = !leftLegActive;


        }

        //initializes the components of the leg to their retracted state
        public void LegInit (IMyExtendedPistonBase xPiston, IMyExtendedPistonBase yPiston, IMyLandingGear magPlate)
        {
            if(!magPlate.IsLocked)
                yPiston.Retract(); 
            xPiston.Retract();
        }

        //steps the leg with the given pistons returns true if the step was properly executed 
        public bool StepLeg(IMyExtendedPistonBase xPiston, IMyExtendedPistonBase yPiston, IMyLandingGear steppingMagPlate, IMyLandingGear retractingMagPlate)
        {

            yPiston.Extend();
            steppingMagPlate.Lock();

            //if plate is locked a successful step was made, otherwise the step was not successful 
            if (steppingMagPlate.IsLocked)
            {
                retractingMagPlate.Unlock();
                xPiston.Extend();
                return true;
            }
            else
                return false;
        }

        public void ResetLeg(IMyExtendedPistonBase xPiston, IMyExtendedPistonBase yPiston)
        {
            yPiston.Retract();
            xPiston.Retract();
        }

        //

    }
}
