using Assets.AltUnityTester.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{

    public abstract class AltUnityCommand
    {
        protected string[] Parameters;

        public string MessageId { get { return Parameters[0]; } }
        protected string CommandName { get { return Parameters[1]; } }

        protected static bool LogEnabled = false;
        private string logMessage;


        protected AltUnityCommand(string[] parameters, int expectedParametersCount)
        {
            validateParametersCount(parameters, expectedParametersCount);
            this.Parameters = parameters;
            BeginLog();
        }

        public string GetLogMessage()
        {
            return logMessage;
        }

        public void EndLog(string message)
        {
            if (!string.IsNullOrEmpty(logMessage))
            {
                AltUnityRunner.ServerLogger.Write(logMessage);
                UnityEngine.Debug.Log(logMessage);
            }

            AltUnityRunner.ServerLogger.WriteLine("response sent: " + message);
            UnityEngine.Debug.Log(message);

            logMessage = string.Empty;
        }

        public void SendResponse(AltClientSocketHandler handler)
        {
            AltUnityRunner._responseQueue.ScheduleResponse(delegate
            {
                string response = null;
                try
                {
                    response = Execute();
                }
                catch (System.NullReferenceException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorNullRefferenceMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.FailedToParseArgumentsException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorFailedToParseArguments;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.MethodWithGivenParametersNotFoundException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorMethodWithGivenParametersNotFound;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.InvalidParameterTypeException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorInvalidParameterType;
                }
                catch (Newtonsoft.Json.JsonException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorCouldNotParseJsonString;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorComponentNotFoundMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.MethodNotFoundException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorMethodNotFoundMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorPropertyNotFoundMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.AssemblyNotFoundException e)
                {
                    UnityEngine.Debug.Log(e);
                    response = AltUnityErrors.errorAssemblyNotFoundMessage;
                }
                catch (System.Exception exception)
                {
                    UnityEngine.Debug.Log(exception);
                    response = AltUnityErrors.errorUnknownError + AltUnityRunner._altUnityRunner.requestSeparatorString + exception;
                }

                finally
                {
                    handler.SendResponse(this, response);
                }
            });
        }
        public abstract string Execute();

        protected void validateParametersCount(string[] parameters, int expectedCount)
        {
            if (parameters.Length != expectedCount)
            {
                throw new InvalidParametersOnDriverCommandException("Expected " + expectedCount + " parameters, got " + parameters.Length);
            }
        }
        protected void LogMessage(string message)
        {
            if (LogEnabled)
            {
                this.logMessage += System.DateTime.Now + ":" + message + System.Environment.NewLine;
            }
        }

        private void BeginLog()
        {
            logMessage = string.Empty;
            var message = "command received: " + string.Join(";", Parameters);

            AltUnityRunner.ServerLogger.WriteLine(message);
            UnityEngine.Debug.Log(message);
        }
    }
}