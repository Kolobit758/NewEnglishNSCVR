public class SkillSession
{
    public int turnCount;
    public int meaningfulTurnCount;

    public void AddTurn(SkillFeature f)
    {
        turnCount++;

        if (f.hasMeaning)
            meaningfulTurnCount++;
    }

    public void Reset()
    {
        turnCount = 0;
        meaningfulTurnCount = 0;
    }
}