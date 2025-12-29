using System;

namespace NeuralNetwork1
{
    public class StudentNetwork : BaseNetwork
    {
        private readonly int[] structure;
        private readonly int layersCount;

        private readonly double[][][] weights;
        private readonly double[][] biases;
        private readonly double[][] outputs;
        private readonly double[][] deltas;

        private double learningRate = 0.01;
        private readonly Random rnd = new Random();

        public StudentNetwork(int[] structure)
        {
            if (structure == null || structure.Length < 2)
                throw new ArgumentException("Структура сети должна содержать минимум входной и выходной слой.");

            this.structure = (int[])structure.Clone();
            layersCount = structure.Length;

            weights = new double[layersCount][][];
            biases = new double[layersCount][];
            outputs = new double[layersCount][];
            deltas = new double[layersCount][];

            outputs[0] = new double[structure[0]];
            deltas[0] = null;
            biases[0] = null;
            weights[0] = null;

            for (int l = 1; l < layersCount; l++)
            {
                int neurons = structure[l];
                int prev = structure[l - 1];

                weights[l] = new double[neurons][];
                for (int i = 0; i < neurons; i++)
                {
                    weights[l][i] = new double[prev];

                    double limit = Math.Sqrt(6.0 / (prev + neurons));
                    for (int j = 0; j < prev; j++)
                        weights[l][i][j] = RandUniform(-limit, limit);
                }

                biases[l] = new double[neurons];
                outputs[l] = new double[neurons];
                deltas[l] = new double[neurons];
            }
        }

        private double RandUniform(double min, double max)
        {
            return min + rnd.NextDouble() * (max - min);
        }

        private static double Sigmoid(double x)
        {
            if (x < -40.0) return 0.0;
            if (x > 40.0) return 1.0;
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        private static double SigmoidDerivFromY(double y)
        {
            return y * (1.0 - y);
        }

        protected override double[] Compute(double[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Length != structure[0])
                throw new ArgumentException($"Ожидается входной вектор длиной {structure[0]}");

            for (int i = 0; i < input.Length; i++)
                outputs[0][i] = input[i];

            for (int l = 1; l < layersCount; l++)
            {
                int neurons = structure[l];
                int prev = structure[l - 1];

                double[] prevOut = outputs[l - 1];
                double[] curOut = outputs[l];
                double[] b = biases[l];
                double[][] w = weights[l];

                for (int i = 0; i < neurons; i++)
                {
                    double sum = b[i];
                    double[] wRow = w[i];
                    for (int j = 0; j < prev; j++)
                        sum += wRow[j] * prevOut[j];

                    curOut[i] = Sigmoid(sum);
                }
            }

            int outSize = structure[layersCount - 1];
            double[] result = new double[outSize];
            Array.Copy(outputs[layersCount - 1], result, outSize);
            return result;
        }

        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            if (sample == null)
                throw new ArgumentNullException(nameof(sample));

            double[] output = Compute(sample.input);
            sample.ProcessPrediction(output);
            Backpropagate(sample);

            return 1;
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount,
                                              double acceptableError, bool parallel)
        {
            if (samplesSet == null)
                throw new ArgumentNullException(nameof(samplesSet));
            if (samplesSet.Count == 0)
                return 0.0;

            double error = double.MaxValue;
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int epoch = 1; epoch <= epochsCount && error > acceptableError; epoch++)
            {
                error = 0.0;

                foreach (var sample in samplesSet.samples)
                {
                    double[] output = Compute(sample.input);
                    sample.ProcessPrediction(output);
                    Backpropagate(sample);
                    error += sample.EstimatedError();
                }

                error /= samplesSet.Count;

                OnTrainProgress((double)epoch / epochsCount, error, watch.Elapsed);
            }

            watch.Stop();
            OnTrainProgress(1.0, error, watch.Elapsed);
            return error;
        }

        private void Backpropagate(Sample sample)
        {
            int lastLayer = layersCount - 1;
            int outSize = structure[lastLayer];

            double[] target = new double[outSize];
            int cls = (int)sample.actualClass;
            if (cls >= 0 && cls < outSize)
                target[cls] = 1.0;

            double[] yOut = outputs[lastLayer];
            double[] deltaOut = deltas[lastLayer];

            for (int j = 0; j < outSize; j++)
            {
                double y = yOut[j];
                double d = target[j];
                deltaOut[j] = SigmoidDerivFromY(y) * (d - y);
            }

            for (int l = lastLayer - 1; l >= 1; l--)
            {
                int neurons = structure[l];
                int nextNeurons = structure[l + 1];

                double[] curOut = outputs[l];
                double[] curDelta = deltas[l];
                double[][] wNext = weights[l + 1];
                double[] deltaNext = deltas[l + 1];

                for (int i = 0; i < neurons; i++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < nextNeurons; k++)
                        sum += wNext[k][i] * deltaNext[k];

                    curDelta[i] = SigmoidDerivFromY(curOut[i]) * sum;
                }
            }

            for (int l = 1; l < layersCount; l++)
            {
                int neurons = structure[l];
                int prev = structure[l - 1];

                double[] prevOut = outputs[l - 1];
                double[] curDelta = deltas[l];
                double[][] w = weights[l];
                double[] b = biases[l];

                for (int i = 0; i < neurons; i++)
                {
                    b[i] += learningRate * curDelta[i];

                    double[] wRow = w[i];
                    for (int j = 0; j < prev; j++)
                        wRow[j] += learningRate * curDelta[i] * prevOut[j];
                }
            }
        }
    }
}
