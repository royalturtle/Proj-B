using UnityEngine;

public class GroundCameraSet {
    public GroundCameraData Default {get; private set;}
    public GroundCameraData Pitching {get; private set;}
    public GroundCameraData C {get; private set;}
    public GroundCameraData B1 {get; private set;}
    public GroundCameraData B2 {get; private set;}
    public GroundCameraData B3 {get; private set;}
    public GroundCameraData B2Relay {get; private set;}
    public GroundCameraData SSRelay {get; private set;}
    public GroundCameraData LF {get; private set;}
    public GroundCameraData CF {get; private set;}
    public GroundCameraData RF {get; private set;}

    public GroundCameraSet() {
        Default     = new GroundCameraData();
        /*
        Pitching    = new GroundCameraData(            y : 0.2f , size : 2.0f);
        C           = new GroundCameraData(                       size : 2.0f);
        B1          = new GroundCameraData(x :  0.5f , y : 0.55f, size : 2.0f);
        B2          = new GroundCameraData(            y : 1.2f , size : 2.0f);
        B3          = new GroundCameraData(x : -0.5f , y : 0.55f, size : 2.0f);
        B2Relay     = new GroundCameraData(x :  0.6f , y : 1.5f , size : 2.0f);
        SSRelay     = new GroundCameraData(x : -0.6f , y : 1.5f , size : 2.0f);
        */
        Pitching    = new GroundCameraData(            y : 0.2f , size : 1.8f);
        C           = Pitching;
        B1          = Pitching;
        B2          = Pitching;
        B3          = Pitching;
        B2Relay     = Pitching;
        SSRelay     = Pitching;
        LF          = new GroundCameraData(x : -1.2f , y : 1.6f , size : 1.5f);
        CF          = new GroundCameraData(            y : 2.0f , size : 1.5f);
        RF          = new GroundCameraData(x :  1.2f , y : 1.6f , size : 1.5f);
    }

    public GroundCameraData GetPosition(BatterPositionEnum position) {
        switch(position) {
            case BatterPositionEnum.C : return C;
            case BatterPositionEnum.B1: return B1;
            case BatterPositionEnum.B2: return B2Relay;
            case BatterPositionEnum.B3: return B3;
            case BatterPositionEnum.SS: return SSRelay;
            case BatterPositionEnum.LF: return LF;
            case BatterPositionEnum.CF: return CF;
            case BatterPositionEnum.RF: return RF;
            default:                    return Default;
        }
    }

    public GroundCameraData GetPosition(ThrowingLocation location) {
        switch(location) {
            case ThrowingLocation.C         : return C;
            case ThrowingLocation.B1        : return B1;
            case ThrowingLocation.B2_RELAY  : return B2Relay;
            case ThrowingLocation.B2        : return B2;
            case ThrowingLocation.SS_RELAY  : return SSRelay;
            case ThrowingLocation.B3        : return B3;
            case ThrowingLocation.LF        : return LF;
            case ThrowingLocation.CF        : return CF;
            case ThrowingLocation.RF        : return RF;
            default                         : return Default;
        }
    }

    public GroundCameraData GetBase(int index) {
        switch(index) {
            case 0 :
            case 4 : return C;
            case 1 : return B1;
            case 2 : return B2;
            case 3 : return B3;
            default: return Default;
        }
    }
}
