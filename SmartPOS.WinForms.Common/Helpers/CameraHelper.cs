namespace SmartPOS.WinForms.Common.Helpers
{
    public static class CameraHelper
    {
        public static bool IsValidCameraIndex(int cameraIndex)
        {
            return cameraIndex >= 0;
        }
    }
}