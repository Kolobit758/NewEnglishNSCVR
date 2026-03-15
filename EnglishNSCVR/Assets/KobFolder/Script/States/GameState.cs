public enum gamestate
{
    NPC_comming,
    NPC_Order,
    Player_Order,// do order state for each order and submit to cooker
    Wait_Cooking,// submited to cooker and wait for meal
    Player_Serve,
    NPC_Paid,
    NPC_out,
}