namespace Scripts
{
	[System.Flags]
    public enum TileType : short
    {
        None = 0,
		Down = 1,
		Left = 2,
		Up = 4,
		Right = 8,
    }
}