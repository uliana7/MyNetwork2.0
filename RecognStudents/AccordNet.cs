using System;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.ActivationFunctions;

namespace AForge.WindowsForms
{
    public class AccordNet : BaseNetwork
    {
        private readonly ActivationNetwork net;
        private readonly BackPropagationLearning teacher;

        public AccordNet(int[] structure)
        {
            if (structure == null || structure.Length < 2)
                throw new ArgumentException("structure must have at least 2 layers: input..output");

            if (structure[structure.Length - 1] != Symbols.ClassesCount)
                throw new ArgumentException("Last layer must be equal to Symbols.ClassesCount (6).");

            int inputsCount = structure[0];
            int[] neurons = new int[structure.Length - 1];
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = structure[i + 1];

            net = new ActivationNetwork(new SigmoidFunction(), inputsCount, neurons);
            new NguyenWidrow(net).Randomize();

            teacher = new BackPropagationLearning(net);

            teacher.LearningRate = 0.15;
            teacher.Momentum = 0.0;
        }

        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            if (sample == null)
                throw new ArgumentNullException(nameof(sample));

            int classes = Symbols.ClassesCount;

            double[] output = new double[classes];
            int cls = (int)sample.actualClass;
            if (cls >= 0 && cls < classes)
                output[cls] = 1.0;

            teacher.Run(sample.input, output);
            return 1;
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount, double acceptableError, bool parallel)
        {
            if (samplesSet == null)
                throw new ArgumentNullException(nameof(samplesSet));
            if (samplesSet.Count == 0)
                return 0.0;

            int classes = Symbols.ClassesCount;

            double[][] inputs = new double[samplesSet.Count][];
            double[][] outputs = new double[samplesSet.Count][];

            for (int i = 0; i < samplesSet.Count; i++)
            {
                inputs[i] = samplesSet[i].input;

                outputs[i] = new double[classes];
                int cls = (int)samplesSet[i].actualClass;
                if (cls >= 0 && cls < classes)
                    outputs[i][cls] = 1.0;
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            double error = double.MaxValue;

            // Перемешивание по эпохам
            int[] idx = new int[samplesSet.Count];
            for (int i = 0; i < idx.Length; i++) idx[i] = i;
            var rnd = new Random(123);

            for (int epoch = 1; epoch <= epochsCount; epoch++)
            {
                Shuffle(idx, rnd);

                double sumErr = 0.0;
                for (int k = 0; k < idx.Length; k++)
                {
                    int i = idx[k];
                    sumErr += teacher.Run(inputs[i], outputs[i]);
                }

                error = sumErr / idx.Length;

                OnTrainProgress((double)epoch / epochsCount, error, watch.Elapsed);

                if (error <= acceptableError)
                    break;
            }

            watch.Stop();
            OnTrainProgress(1.0, error, watch.Elapsed);
            return error;
        }

        protected override double[] Compute(double[] input)
        {
            double[] y = net.Compute(input);

            double sum = 0.0;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] < 0.0) y[i] = 0.0;
                sum += y[i];
            }

            if (sum > 1e-12)
            {
                for (int i = 0; i < y.Length; i++)
                    y[i] /= sum;
            }

            return y;
        }

        private static void Shuffle(int[] a, Random rnd)
        {
            for (int i = a.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int t = a[i];
                a[i] = a[j];
                a[j] = t;
            }
        }
    }
}
