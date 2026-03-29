public static class PlayerGamemode 
{
    public enum PlayerMode { Free, Premium }
    
    // ใช้ static เพื่อให้เรียกจาก Script ไหนก็ได้
    public static PlayerMode currentMode = PlayerMode.Free;
}