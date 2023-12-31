﻿// Statuses.ControllerState


namespace CashSwift.Library.Standard.Statuses
{
    public enum ControllerState
    {
        NONE,
        INIT,
        IDLE,
        DROP,
        DROP_PAUSED,
        ESCROW_DROP,
        ESCROW_REJECT,
        DISPENSE,
        OUT_OF_ORDER,
        ESCROWJAM_START,
        ESCROWJAM_OPEN_REQUEST,
        ESCROWJAM_CLEAR_WAIT,
        ESCROWJAM_END_REQUEST,
        ESCROWJAM_END,
    }
}
