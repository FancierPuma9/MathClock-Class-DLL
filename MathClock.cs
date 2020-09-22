using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MathClock
{
    public class MathClock
    {
        private static DateTime curTime;
        private static DateTime alarmTime;
        private static int waitTimeInt;
        public DateTime CurTime
        {
            get { return curTime; }
            set { curTime = value; }
        }
        public DateTime AlarmTime
        {
            get { return alarmTime; }
            set { alarmTime = value; }
        }
        public int WaitTimeInt
        {
            get { return waitTimeInt; }
            set { waitTimeInt = value; }
        }


        private static bool alarmOn;
        private static bool alarmReachedBool;
        private static bool stopLoop = false;
        public bool AlarmOn
        {
            get { return alarmOn; }
            set { alarmOn = value; }
        }
        public bool AlarmReachedBool
        {
            get { return alarmReachedBool; }
            set { alarmReachedBool = value; }
        }
        public bool StopLoop
        {
            get { return stopLoop; }
            set { stopLoop = value; }
        }

        //Variables related to solving the problem
        private static int num1;
        private static int num2;
        private static int answer;
        private static bool completed;
        public int Num1
        {
            get { return num1; }
            set { num1 = value; }
        }
        public int Num2
        {
            get { return num2; }
            set { num2 = value; }
        }
        public int Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }


        private static System.Timers.Timer curTimeTimer;
        private static System.Timers.Timer soundReplayTimer;

        private static int soundReplayTime;
        private static string soundFile;
        public int SoundReplayTime
        {
            get { return soundReplayTime; }
            set { soundReplayTime = value; }
        }
        public string SoundFile
        {
            get { return soundFile; }
            set { soundFile = value; }
        }

        SoundPlayer alarmSound = new SoundPlayer();

        private static System.Timers.Timer alarmTimer;


        public void curTimeStartCall()
        {
            curTimeTimer = new Timer();
            curTimeTimer.Interval = 1000;
            curTimeTimer.Elapsed += curTimerElapsed;
            curTimeTimer.Start();

            return;
        }
        private void curTimerElapsed(object sender, ElapsedEventArgs e)
        {
            curTime = DateTime.Now;
        }

        public void createProblem()
        {
            var rand = new Random();
            num1 = rand.Next(25, 100);
            num2 = rand.Next(25, 100);

            answer = num1 + num2;

            return;
        }

        public void startAlarm()
        {
            alarmOn = true;
            if (alarmTime < curTime)
            {
                Console.WriteLine("The Alarm Time Chosen has already passed please choose another time.");
                alarmOn = false;
                return;
            }
            else if (alarmTime > curTime)
            {
                System.TimeSpan waitTime = alarmTime.Subtract(DateTime.Now);
                waitTimeInt = (int)waitTime.TotalMilliseconds;
                alarmTimer = new Timer();
                alarmTimer.Interval = waitTimeInt;
                alarmTimer.Elapsed += alarmReached;
                alarmTimer.Start();
                createProblem();

            }
        }
        private async void alarmReached(object sender, ElapsedEventArgs e)
        {

            if (stopLoop == false)
            {
                alarmTimer.Stop();

                alarmReachedBool = true;
                soundReplayTimer = new Timer();
                soundReplayTimer.Interval = soundReplayTime;
                soundReplayTimer.Elapsed += restartPlayingSound;
                alarmSound.SoundLocation = soundFile;
                soundReplayTimer.Start();
                stopLoop = true;
            }

            Task<bool> taskDone = Task.Run(() => problemWait());
            await taskDone;

            alarmOn = false;
            completed = false;
            alarmReachedBool = false;
            stopLoop = false;
            soundReplayTimer.Stop();
        }
        private bool problemWait()
        {
            while (alarmOn == true && completed == false)
            {

            }
            bool loopComplete = true;
            return loopComplete;
        }
        private void restartPlayingSound(object sender, ElapsedEventArgs e)
        {
            alarmSound.Play();
        }

        public string testAnswer(int answerFrom)
        {
            string correct;
            if (answer == answerFrom)
            {
                completed = true;
                alarmOn = false;
                correct = "Correct!";
                return correct;
            }
            else
            {
                Console.WriteLine("That answer is incorrect, try again.");
                correct = "That answer is incorrect, try again.";
                completed = false;
                return correct;
            }
        }

        public void stopAlarm()
        {
            if (alarmTimer.Enabled == true)
            {
                alarmTimer.Stop();
            }
            if (alarmReachedBool == true)
            {
                soundReplayTimer.Stop();
            }
            alarmOn = false;
            completed = false;
            alarmReachedBool = false;
            stopLoop = false;

        }
    }
}
