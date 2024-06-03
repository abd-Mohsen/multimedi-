using System;
using System.IO;
using System.Windows.Forms;
using MaterialSkin.Controls;
using NAudio.Wave;

public class AudioForm : MaterialForm
{
    private WaveInEvent waveIn;
    private WaveFileWriter writer;
    private MemoryStream memoryStream;
    private WaveOutEvent waveOut;
    private System.Windows.Forms.Timer timer;
    private DateTime startTime;
    private MaterialButton recordButton = new();
    private MaterialButton stopButton = new();
    private MaterialButton playButton = new();
    private MaterialButton saveButton = new();

    public MemoryStream RecordedAudio { get; private set; }

    public AudioForm()
    {
        //InitializeComponent();

        timer = new()
        {
            Interval = 1000
        };
        timer.Tick += Timer_Tick;

        recordButton.Click += StartRecording;
        stopButton.Click += StopRecordingButton_Click;
        playButton.Click += PlayRecordingButton_Click;
        saveButton.Click += SaveRecordingButton_Click;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        TimeSpan elapsed = DateTime.Now - startTime;
        //recordingTimeLabel.Text = elapsed.ToString(@"hh\:mm\:ss");
    }

    private void StartRecording(object? sender, EventArgs e)
    {
        memoryStream = new MemoryStream();
        waveIn = new()
        {
            WaveFormat = new WaveFormat(44100, 1)
        };
        waveIn.DataAvailable += WaveIn_DataAvailable;
        //writer = new WaveFileWriter(new IgnoreDisposeStream(memoryStream), waveIn.WaveFormat);

        waveIn.StartRecording();
        startTime = DateTime.Now;
        timer.Start();
    }

    private void StopRecordingButton_Click(object sender, EventArgs e)
    {
        waveIn.StopRecording();
        waveIn.Dispose();
        writer.Close();
        writer.Dispose();
        timer.Stop();

        memoryStream.Seek(0, SeekOrigin.Begin);
        RecordedAudio = new MemoryStream(memoryStream.ToArray());
    }

    private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        writer.Write(e.Buffer, 0, e.BytesRecorded);
    }

    private void PlayRecordingButton_Click(object sender, EventArgs e)
    {
        if (RecordedAudio != null)
        {
            RecordedAudio.Seek(0, SeekOrigin.Begin);
            waveOut = new WaveOutEvent();
            waveOut.Init(new WaveFileReader(RecordedAudio));
            waveOut.Play();
        }
        else
        {
            MessageBox.Show("No recording available to play.");
        }
    }

    private void SaveRecordingButton_Click(object sender, EventArgs e)
    {
        if (RecordedAudio != null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "WAV files (*.wav)|*.wav";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, RecordedAudio.ToArray());
                MessageBox.Show("Recording saved.");
            }
        }
        else
        {
            MessageBox.Show("No recording available to save.");
        }
    }
}
