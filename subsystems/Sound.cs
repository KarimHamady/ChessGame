using System.Media;

namespace ChessGame.Subsystems
{
    internal class Sound
    {
        SoundPlayer MoveSound {  get; set; }
        SoundPlayer CheckSound { get; set; }
        SoundPlayer CheckmateSound { get; set; }
        SoundPlayer CastlingSound { get; set; }
        SoundPlayer CaptureSound { get; set; }
        SoundPlayer StartSound { get; set; }
        public Sound()
        {
            MoveSound = new("../../../data/move_sound.wav");
            CheckSound = new("../../../data/check_sound.wav");
            CheckmateSound = new("../../../data/checkmate_sound.wav");
            CastlingSound = new("../../../data/castling_sound.wav");
            CaptureSound = new("../../../data/capture_sound.wav");
            StartSound = new("../../../data/game_start.wav");
        }
        public void PlayMoveSound() { MoveSound.Play(); }
        public void PlayCheckSound() { CheckSound.Play(); }
        public void PlayCheckmateSound() { CheckmateSound.Play(); }
        public void PlayCastlingSound() { CastlingSound.Play(); }
        public void PlayCaptureSound() { CaptureSound.Play(); }
        public void PlayStartSound() { StartSound.Play(); }
    }
}
