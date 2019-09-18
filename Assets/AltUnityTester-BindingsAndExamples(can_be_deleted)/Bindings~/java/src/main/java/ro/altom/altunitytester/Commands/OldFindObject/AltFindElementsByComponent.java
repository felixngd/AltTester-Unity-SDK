package ro.altom.altunitytester.Commands.OldFindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltFindElementsByComponent extends AltCommandReturningAltObjects {
    private AltFindElementsByComponentParameters altFindElementsByComponentParameters;
    public AltFindElementsByComponent(AltBaseSettings altBaseSettings, AltFindElementsByComponentParameters altFindElementsByComponentParameters) {
        super(altBaseSettings);
        this.altFindElementsByComponentParameters = altFindElementsByComponentParameters;
    }
    public AltUnityObject[] Execute(){
        send(CreateCommand("findObjectsByComponent",altFindElementsByComponentParameters.getAssemblyName(), altFindElementsByComponentParameters.getComponentName(), altFindElementsByComponentParameters.getCameraName(), String.valueOf(altFindElementsByComponentParameters.isEnabled())));
        return ReceiveListOfAltUnityObjects();
    }
}
