using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    //Uses GameService UI page to show UI information
    //And uses Game Service Manager to get currently loaded/initialized service count number
    public sealed class SplashScreenManager : GameService
    {
        protected internal override void OnInit()
        {
            base.OnInit();
            //fetch proper splash UI page from GameServiceManager
            //Now we bind information from events;

            GameServiceManager.OnInitializedAGameService.AddListener((startedServiceNum, totalServiceNum, service) =>
            {
                var serviceAbout = service.AboutService;
                var serviceDesc = service.ServiceDescription;
                var overallInitProgress = (float)startedServiceNum / (float)totalServiceNum;

                //Now bind those info to UI

            });
        }

        protected internal override void OnTick()
        {
            Debug.Log("yu hu!");
        }
    }
}