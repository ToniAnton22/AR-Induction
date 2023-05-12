using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace FrameworkDesign.Example

{



    public class PlaceManager : MonoBehaviour

    {

        [Header("AR Foundation")]

        /// <summary>

        /// The active ARRaycastManager used in the example.

        /// </summary>

        public ARRaycastManager m_RaycastManager;





        [Header("UI")]

        [SerializeField]

        [Tooltip("Instantiates this prefab on a plane at the touch location.")]

        GameObject m_PlacedPrefab;//The prefab to be placed



        /// <summary>

        /// The prefab to instantiate on touch.

        /// </summary>

        public GameObject placedPrefab

        {

            get { return m_PlacedPrefab; }

            set { m_PlacedPrefab = value; }

        }

        [HideInInspector]

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();//store the detected collision point

        /// <summary>

        /// The object instantiated as a result of a successful raycast intersection with a plane.

        /// </summary>

        public GameObject spawnedObject { get; private set; }



        void Awake()

        {

            // m_RaycastManager = GetComponent<ARRaycastManager> ();//ARRaycastManager can also be obtained through GetComponent

        }



        bool TryGetTouchPosition(out Vector2 touchPosition)

        {

            if (Input.touchCount > 0)

            {

                touchPosition = Input.GetTouch(0).position;

                return true;

            }



            touchPosition = default;

            return false;

        }



        void Update()

        {

            if (!TryGetTouchPosition(out Vector2 touchPosition))

                return;



            var touch = Input.GetTouch(0);

            const TrackableType trackableTypes =

            TrackableType.FeaturePoint |

            TrackableType.PlaneWithinPolygon;



            if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved)//Move the placed object

            {



                if (m_RaycastManager.Raycast(touchPosition, s_Hits, trackableTypes))

                {

                    // Raycast hits are sorted by distance, so the first one

                    // will be the closest hit.

                    var hitPose = s_Hits[0].pose;

                    if (spawnedObject != null)

                    {

                        spawnedObject.transform.position = hitPose.position;

                    }

                }

            }

            if (Input.touchCount == 1 && touch.phase == TouchPhase.Began)//Detect touch begin, do ray collision detection in touch begin

            {

                //---Determine whether to touch the UI component----

                //#if IPHONE || ANDROID

                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))

                //#else

                // if (EventSystem.current.IsPointerOverGameObject())

                //#endif

                //Debug.Log("The current touch is on the UI");

                {

                    Debug.Log($"The current touch is on the UI" + touch.phase);
                   
                    return;

                }

                else

                {

                    //Debug.Log("There is currently no touch on the UI");

                    Debug.Log($"Currently no touch on UI" + touch.phase);

                }



                if (m_RaycastManager.Raycast(touchPosition, s_Hits, trackableTypes))

                {

                    // Raycast hits are sorted by distance, so the first one

                    // will be the closest hit.

                    var hitPose = s_Hits[0].pose;

                    if (spawnedObject == null)

                    {

                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);//Instantiate the prefab object

                    }

                    else

                    {

                        spawnedObject.transform.position = hitPose.position;//update object state

                    }

                }

            }

        }

    }
}
