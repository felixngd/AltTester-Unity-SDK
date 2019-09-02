﻿namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetAllCameras : Command
    {
        public override string Execute()
        {
            UnityEngine.Debug.Log("getAllCameras");
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
                var cameras = UnityEngine.GameObject.FindObjectsOfType<UnityEngine.Camera>();
                System.Collections.Generic.List<string> cameraNames = new System.Collections.Generic.List<string>();
                foreach (UnityEngine.Camera camera in cameras)
                {
                    cameraNames.Add(camera.name);
                }
            response = Newtonsoft.Json.JsonConvert.SerializeObject(cameraNames);
            return response;
        }
    }
}
