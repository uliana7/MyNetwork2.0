using System.Drawing;
using System.Threading.Tasks;

namespace AForge.WindowsForms
{
    public delegate void FormUpdateDelegate();

    class Controller
    {
        private readonly FormUpdateDelegate formUpdateDelegate;

        public MagicEye processor = new MagicEye();

        public Settings settings
        {
            get { return processor.settings; }
            set { processor.settings = value; }
        }

        private bool _imageProcessed = true;
        public bool Ready { get { return _imageProcessed; } }

        public double[] LastSensors { get; private set; }

        public Controller(FormUpdateDelegate updater)
        {
            formUpdateDelegate = updater;
        }

        public async Task<bool> ProcessImage(Bitmap image)
        {
            if (image == null) return false;

            if (!Ready)
            {
                image.Dispose();
                return false;
            }

            _imageProcessed = false;

            try
            {
                bool ok = await Task.Run(() => processor.ProcessImage(image));

                if (!ok || processor.lastSensors == null)
                    LastSensors = null;
                else
                    LastSensors = (double[])processor.lastSensors.Clone();

                formUpdateDelegate?.Invoke();
                return ok;
            }
            catch
            {
                LastSensors = null;
                return false;
            }
            finally
            {
                _imageProcessed = true;
                image.Dispose();
            }
        }

        public Bitmap GetOriginalImage() => processor.original;
        public Bitmap GetProcessedImage() => processor.processed;
    }
}
