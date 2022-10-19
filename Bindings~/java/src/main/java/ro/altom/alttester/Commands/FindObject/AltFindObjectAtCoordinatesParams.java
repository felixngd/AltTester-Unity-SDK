package ro.altom.alttester.Commands.FindObject;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.position.Vector2;

public class AltFindObjectAtCoordinatesParams extends AltMessage {

    public static class Builder {
        private Vector2 coordinates;

        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;
        }

        public AltFindObjectAtCoordinatesParams build() {
            AltFindObjectAtCoordinatesParams params = new AltFindObjectAtCoordinatesParams();
            params.coordinates = this.coordinates;
            return params;
        }
    }

    private Vector2 coordinates;

    private AltFindObjectAtCoordinatesParams() {
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
    }
}