using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ScanChecker;
using ProductProccessing;
using ZXing;
using System.Threading.Tasks;

namespace AndroidScanner
{
    public class AndroidScanner : MonoBehaviour
    {

        [SerializeField]
        private ARSession session;
        [SerializeField]
        private ARSessionOrigin sessionOrigin;
        [SerializeField]
        private ARCameraManager cameraManager;
        [SerializeField]
        private TextMeshProUGUI boxTitle;
        [SerializeField]
        private TextMeshProUGUI cageNumber;
        [SerializeField]
        private GameObject numOfProducts;
        [SerializeField]
        private GameObject scanOk;
        [SerializeField]
        private GameObject scanNotOk;
        [SerializeField]
        private TextMeshProUGUI currentStage;
        private static string stage;

        private Result result;
        Thread decoderThread;
        public static bool assignmentStart;
        public static Assignment currentAssignment;

        public int frameIncrement = 1;
        public int frameLimit = 10;

        Coroutine myCoroutine;

        private Texture2D cameraImageTexture;

        private readonly IBarcodeReader barcodeReader = new BarcodeReader
        {
            AutoRotate = false,
            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = false
            }
        };

        public static Assignment StartAssignment(Assignment assignment)
        {
            try
            {
               
                assignmentStart = true;
                Debug.Log("SCANNER STARTED" + assignment.aisle);
                currentAssignment = assignment;
                Debug.Log("SCANNER STARTED" + currentAssignment.orders[0].productName);
              
                stage = "Box";
   
               
                return assignment;
            }
            catch (System.Exception e)
            {
                Debug.LogError("ASSIGNMENT ERROR AT START " + " " + e.Message);
                assignmentStart = false;
                return null;
            }
        }
        private void OnEnable()
        {
            cameraManager.frameReceived += OnCameraFrameReceived;
        }
        private void OnDisable()
        {
            cameraManager.frameReceived -= OnCameraFrameReceived;
        }

        private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            try
            {
                if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
                {
                    return;
                }


                var conversionParams = new XRCpuImage.ConversionParams
                {
                    //Get entire image
                    inputRect = new RectInt(0, 0, image.width, image.height),

                    //Downsample by 2
                    outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

                    //Choose RGBA format
                    outputFormat = TextureFormat.ARGB32,

                    // Flip across the vertical axis (mirror image)
                    transformation = XRCpuImage.Transformation.MirrorY
                };

                // See how many bytes you need to store the final image
                int size = image.GetConvertedDataSize(conversionParams);

                //Allocate a buffer to store the image
                var buffer = new NativeArray<byte>(size, Allocator.Temp);

                //Extract the image data
                image.Convert(conversionParams, buffer);

                //The image was converted to RBA32 format and written into provided buffer
                // so you can dispose of the XRCPUIMAGE. You must do this or it will leak resources.
                image.Dispose();

                if (!assignmentStart)
                {
                    Debug.Log("SCANNER Stooped" + " " + assignmentStart);
                    return;
                }

                // At this point, you can process the image, pass it to a computer vision algorith, etc,
                // In this example, you apply it to a texture to visualize it

                // You've got the data, lets put it into a texture so you can visualzie it.
                cameraImageTexture = new Texture2D(
                    conversionParams.outputDimensions.x,
                    conversionParams.outputDimensions.y,
                    conversionParams.outputFormat,
                    false);

                cameraImageTexture.LoadRawTextureData(buffer);
                cameraImageTexture.Apply();

                //Done with your temporary data, so you can dispose it.
                buffer.Dispose();
                Debug.Log("SCANNER BEGINNING");
                //Detect and decode the barcode inside the bitmap
                result = barcodeReader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
                Debug.Log("SCANNER DECODED" + " " + result.ToString());


                if (!numOfProducts.activeInHierarchy)
                {
                    numOfProducts.SetActive(true);
                }

                //Do something with the result

                if (!result.Equals(null))
                {
                    frameIncrement = 1;
                    Debug.Log("SCANNER assignment created");


                    if (stage == "Box")
                    {
                        var success = ScanChecker.ScanChecker.VerifyBox(currentAssignment, result.ToString());

                        Debug.Log("[SCANNER] SEARCHED");

                        if (success != null)
                        {
                            Debug.Log("[SEARCHED] success = " + success);
                            StartCoroutine(DisplayResult(true));
                            currentStage.text = "Product cage";
                            stage = "Cage";
                 
                        }
                        else
                        {
                            StartCoroutine(DisplayResult(false));
                            stage = "Box";
                        }
                    }
                    else if (stage == "Cage")
                    {

                        Debug.Log("[SCANNER] CAGE SEARCH");
                        //var processThread = new Thread(new ThreadStart(() =>
                        // ProductPlaced(scanned[0].order, scanned[0].order.cageNumber)
                        //));
                        //processThread.Start();
                        //processThread.Join();
                        var cageScan = ScanChecker.ScanChecker.VerifyCage(currentAssignment, result.ToString());

                        frameIncrement = 1;
                        if (cageScan != null)
                        {
                            StartCoroutine(DisplayResult(true));

                            currentAssignment.orders.RemoveAt(0);
                            
                            if (currentAssignment.orders.Count == 0)
                            {
                                currentAssignment.Fulfilled(true);
                                assignmentStart = false;
                                currentStage.text = "Assignment complete!";
                                numOfProducts.SetActive(false);
                            }
                            else
                            {
                                currentStage.text = "Scan next Product!";
                                stage = "Box";
                            }

                        }
                        else
                        {
                            StartCoroutine(DisplayResult(false));
                        }
                    }
                    else
                    {

                        Debug.Log("[SCANNER] SEARCHED");
                        //var processThread = new Thread(new ThreadStart(() => ProductPlaced(scanned[0].order, scanned[0].order.cageNumber)));
                        // processThread.Start();
                        // processThread.Join();
                        currentStage.text = "Connecting scanner...";
                        stage = "Box";
                        frameIncrement = 1;

                    }
                }
            }
            catch(System.Exception e)
            {
                Debug.Log("Error message barcode" + e.Message);
            }

        }
        private void Start()
        {

            stage = "None";
            currentStage.text = "Select an assignment in the menu option.";
        }
        /*  public (bool,Order) Search(string boxName, Assignment assignment)
          {   
              foreach (Order verifiedOrder in assignment.orders){
                  Debug.Log("Box value is: " + verifiedOrder);
                  if(verifiedOrder.productName == boxName)
                  {
                      currentStage.text = "Scan cage...";
                      boxTitle.text = "Name product: " + verifiedOrder.productName;
                      cageNumber.text = "Cage number ->" + verifiedOrder.cageNumber;
                      stage = "Cage";
                      numOfProducts.SetActive(true);

                      return (true, verifiedOrder);
                  }
              }
              Debug.Log("[SCANNED] I am called null ");
              return (false,null);
          }*/
        /*  public IEnumerator ProductPlaced(Order order, int cage)
          {

              yield return new WaitUntil(Success);
          }
          bool Success()
          {
              Debug.Log("[COROUTINE] cage order " + order.cageNumber + " cage num" + cage);
              if (order.cageNumber == cage)
              {
                  StartCoroutine(DisplayResult(true));
                  currentStage.text = "Box scanned succesfully";
                  return true;
              }
              else
              {
                  StartCoroutine(DisplayResult(false));
                  return false;
              }

          }*/

        public IEnumerator DisplayResult(bool verify)
        {
            Debug.Log("[SEARCHED] verify = " + verify.ToString());
            if (verify)
            {
                scanOk.SetActive(true);
                yield return new WaitForSeconds(1);
                scanOk.SetActive(false);
            }
            else
            {
                scanNotOk.SetActive(true);
                yield return new WaitForSeconds(1);
                scanNotOk.SetActive(false);
            }
            yield return null;

        }
        // Update is called once per frame
        void Update()
        {
            if (Time.frameCount % frameLimit == 0)
            {
                if(stage == "Box")
                {
                    numOfProducts.SetActive(true);
                }
                if (currentAssignment.orders.Count > 0)
                {
                    boxTitle.text = "Name product: " + currentAssignment.orders[0].productName;
                    cageNumber.text = "Cage number ->" + currentAssignment.orders[0].cageNumber;
                }
                OnEnable();
            }
            else
            {
                OnDisable();
            }
        }
    }
}