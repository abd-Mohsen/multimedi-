using System;
using System.IO;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using NAudio.Wave;

public class AudioForm : MaterialForm
{
    private WaveInEvent waveIn;
    private WaveFileWriter? waveFileWriter;
    private MemoryStream memoryStream;
    private WaveOutEvent waveOut;
    private System.Windows.Forms.Timer timer;
    private DateTime startTime;
    private MaterialButton recordButton = new();
    private MaterialButton stopButton = new();
    private MaterialButton playButton = new();
    private MaterialButton saveButton = new();

    MaterialLabel recordingTimeLabel = new();

    readonly string path = "output/audio.wav";
    readonly string path2 = "output/audio_temp.wav";
    AudioFileReader? audioFileReader;

    public MemoryStream RecordedAudio { get; private set; }

    public AudioForm()
    {
        MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
        materialSkinManager.AddFormToManage(this);
        materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

        timer = new()
        {
            Interval = 1000
        };
        timer.Tick += Timer_Tick;

        recordButton = new(){
            Text = "تسجيل",
            Visible = true,
        };
        recordButton.Click += StartRecording;

        stopButton = new(){
            Text = "توقف",
            Visible = false,
        };
        stopButton.Click += StopRecording;

        playButton = new(){
            Text = "سماع",
            Visible = false,
        };
        playButton.Click += PlayRecord;
       

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
        };
           
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
        
        layout.Controls.Add(recordingTimeLabel, 0, 0);
        layout.Controls.Add(recordButton, 0, 1);
        layout.Controls.Add(stopButton, 0, 2);
        layout.Controls.Add(playButton, 0, 3);

        Controls.Add(layout);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimeSpan elapsed = DateTime.Now - startTime;
        recordingTimeLabel.Text = elapsed.ToString(@"hh\:mm\:ss");
    }

    private void StartRecording(object? sender, EventArgs e)
    {
        // memoryStream = new MemoryStream();
        // waveIn = new()
        // {
        //     WaveFormat = new WaveFormat(44100, 1)
        // };

        // //writer = new WaveFileWriter(new IgnoreDisposeStream(memoryStream), waveIn.WaveFormat);

        // waveIn.StartRecording();

        //delete audio first
        audioFileReader = null;
        if(File.Exists(path)){
            //Thread.Sleep(1000);
            File.Delete(path);
        } 

        waveIn = new()
        {
            WaveFormat = new WaveFormat(44100, 1)
        };
        try{
            waveIn.DataAvailable += (sender, e) => {
                //waveFileWriter = new(new IgnoreDisposeStream(memoryStream), waveIn.WaveFormat);
                waveFileWriter = new(path, waveIn.WaveFormat);
                MessageBox.Show("data available, writer is set");
                waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
            };
        }catch(Exception ex){
            MessageBox.Show(ex.Message);
        }
        
        stopButton.Visible = true;
        recordButton.Visible = false;
        playButton.Visible = false;

        startTime = DateTime.Now;
        timer.Start();

        waveIn.StartRecording();
    }

    private void StopRecording(object? sender, EventArgs e)
    {
        waveFileWriter!.Close();
        waveFileWriter.Dispose();
        waveIn.StopRecording();
        waveIn.Dispose();
        waveFileWriter = null;

        timer.Stop();
        audioFileReader = new(path);

        stopButton.Visible = false;
        playButton.Visible = true;
        recordButton.Visible = true;

        // memoryStream.Seek(0, SeekOrigin.Begin);
        // RecordedAudio = new MemoryStream(memoryStream.ToArray());
        //SaveRecording();
    }

    private void PlayRecord(object? sender, EventArgs e)
    {
        using(WaveOutEvent outputDevice = new()){
            outputDevice.Init(audioFileReader);
            outputDevice.Play();
            while(outputDevice.PlaybackState == PlaybackState.Playing){
                Thread.Sleep(1000);
            }
        }         
    }

    private void SaveRecording()
    {
        if (RecordedAudio != null)
        {
            File.WriteAllBytes(path, RecordedAudio.ToArray());
            MessageBox.Show("Recording saved.");
        }
        else
        {
            MessageBox.Show("No recording available to save.");
        }
    }
}
