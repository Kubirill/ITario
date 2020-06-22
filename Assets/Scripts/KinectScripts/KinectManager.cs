using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Kinect.VisualGestureBuilder;
using Microsoft.Kinect;

public class KinectManager : MonoBehaviour
{

    VisualGestureBuilderDatabase _dbGestures;
    Windows.Kinect.KinectSensor _kinect;
    VisualGestureBuilderFrameSource _gestureFrameSource;
    Windows.Kinect.BodyFrameSource _bodyFrameSource;
    VisualGestureBuilderFrameReader _gestureFrameReader;
    Windows.Kinect.BodyFrameReader _bodyFrameReader;
    Gesture _GLeft; // наш жест
    Gesture _GRight; // наш жест
    Gesture _GUp; // наш жест
    Gesture _GDown; // наш жест
    Windows.Kinect.Body[] _bodies; // все пользователи, найденные Kinect'ом
    Windows.Kinect.Body _currentBody = null; //Текущий пользователь, жесты которого мы отслеживаем
    public string _getsureBasePath = "Right2.gbd"; //Путь до нашей обученной модели
    bool gestureDetected = false;
    public delegate void SimpleEvent();
    public static event SimpleEvent OnSwipeUpDown;
    public static event SimpleEvent Up;
    public static event SimpleEvent Down;
    public static event SimpleEvent Left;
    public static event SimpleEvent Right;
    public static event SimpleEvent Stop;
    // Start is called before the first frame update
    void Start()
    {
        InitKinect();
    }

    void InitKinect()
    {
        _dbGestures = VisualGestureBuilderDatabase.Create(_getsureBasePath);
        _bodies = new Windows.Kinect.Body[6];
        _kinect = Windows.Kinect.KinectSensor.GetDefault();
        _kinect.Open();
        _gestureFrameSource = VisualGestureBuilderFrameSource.Create(_kinect, 0);

        foreach (Gesture gest in _dbGestures.AvailableGestures)
        {
            if (gest.Name == "Left")
            {
                _gestureFrameSource.AddGesture(gest);
                _GLeft = gest;
               // Debug.Log("Added:" + gest.Name);
            }
            if (gest.Name == "Right_Right")
            {
                _gestureFrameSource.AddGesture(gest);
                _GRight = gest;
                //Debug.Log("Added:" + gest.Name);
            }
            if (gest.Name == "Jump")
            {
                _gestureFrameSource.AddGesture(gest);
                _GUp = gest;
               // Debug.Log("Added:" + gest.Name);
            }
            if (gest.Name == "seet")
            {
                _gestureFrameSource.AddGesture(gest);
                _GDown = gest;
                //Debug.Log("Added:" + gest.Name);
            }
        }
        _bodyFrameSource = _kinect.BodyFrameSource;
        _bodyFrameReader = _bodyFrameSource.OpenReader();
        _bodyFrameReader.FrameArrived += _bodyFrameReader_FrameArrived;

        _gestureFrameReader = _gestureFrameSource.OpenReader();
        _gestureFrameReader.IsPaused = true;
        _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
    }
    void _bodyFrameReader_FrameArrived(object sender, Windows.Kinect.BodyFrameArrivedEventArgs args)
    {
        var frame = args.FrameReference;
        using (var multiSourceFrame = frame.AcquireFrame())
        {
            multiSourceFrame.GetAndRefreshBodyData(_bodies); //обновляем данные о найденных людях
            _currentBody = null;
            foreach (var body in _bodies)
            {
                if (body != null && body.IsTracked)
                {
                    _currentBody = body; // для простоты берем первого найденного человека
                    break;
                }
            }
            if (_currentBody != null)
            {
  //              Debug.Log("_currentBody is not null");
                _gestureFrameSource.TrackingId = _currentBody.TrackingId;
                _gestureFrameReader.IsPaused = false;
            }
            else
            {
               // Debug.Log("_currentBody is null");
                _gestureFrameSource.TrackingId = 0;
                _gestureFrameReader.IsPaused = true;
            }

        }
    }

    void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs args)
    {

        if (_gestureFrameSource.IsTrackingIdValid)
        {
            //Debug.Log("Tracking id is valid, value = " + _gestureFrameSource.TrackingId);
            using (var frame = args.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    //using (var results = frame.DiscreteGestureResults)
                    var results = frame.DiscreteGestureResults;
                    if (results != null && results.Count > 0)
                    {
                        DiscreteGestureResult swipeUpDownResult;
                        DiscreteGestureResult leftResult;
                        DiscreteGestureResult rightResult;
                        DiscreteGestureResult upResult;
                        DiscreteGestureResult downResult;
                        results.TryGetValue(_GLeft, out leftResult);
                        results.TryGetValue(_GRight, out rightResult);
                        results.TryGetValue(_GUp, out upResult);
                        results.TryGetValue(_GDown, out downResult);
                        Debug.Log("Result not null, leftResult = " + leftResult.Confidence);
                        Debug.Log("Result not null, rightResult = " + rightResult.Confidence);
                        Debug.Log("Result not null, upResult = " + upResult.Confidence);
                        Debug.Log("Result not null, down  = " + downResult.Confidence);

                        if (leftResult.Confidence > 0.5)
                        {
                            if (!gestureDetected)
                            {
                                gestureDetected = true;
                                //Debug.Log("Left Gesture");
                                
                                    Controll.horizontal_move = -1;
                            }
                        }
                        else
                        {
                            //Debug.Log("False");

                            gestureDetected = false;
                        }


                        if (rightResult.Confidence > 0.3)
                        {
                            if (!gestureDetected)
                            {
                                gestureDetected = true;
                                //Debug.Log("Right Gesture");
                                Controll.horizontal_move = 1;
                            }
                        }
                        else
                        {
                            //Debug.Log("False");

                            gestureDetected = false;
                        }
                        if ((rightResult.Confidence <= 0.3)&& (leftResult.Confidence <= 0.5))
                        {
                            
                            if (!gestureDetected)
                            {
                                gestureDetected = true;
                                Controll.horizontal_move = 0;
                                
                                //Debug.Log("Stop Gesture");
                                //if (Stop != null)
                                    //Controll.horizontal_move = 0 ;
                            }
                        }

                        else
                        {
                            //Debug.Log("False");

                            gestureDetected = false;
                        }

                        
                        if (upResult.Confidence > 0.01)
                        {
                            
                            if (!gestureDetected)
                            {
                                Controll.jumpLong++;
                                gestureDetected = true;
                                //Debug.Log("Up  Gesture");
                                if (Controll.jumpLong<2) Controll.start_up_move = true;
                                else Controll.start_up_move = false;
                            Controll.up_move = true;
                               
                            }
                        }
                        else
                        {
                            //Debug.Log("False");
                            Controll.up_move = false;
                            Controll.jumpLong = 0;
                            gestureDetected = false;
                        }

                        if (downResult.Confidence > 0.2)
                        {
                            
                            if (!gestureDetected)
                            {
                                gestureDetected = true;
                                Controll.down_move = true;
                                //Debug.Log("Up  Gesture");
                                
                            }
                        }
                        else
                        {
                            // Debug.Log("False");
                            Controll.down_move = false;
                            gestureDetected = false;
                        }


                    }
                }
            }
        }
    }

}
