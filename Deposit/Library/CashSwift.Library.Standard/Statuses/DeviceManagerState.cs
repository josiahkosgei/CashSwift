﻿// Statuses.DeviceManagerState


namespace CashSwift.Library.Standard.Statuses
{
    public enum DeviceManagerState
    {
        NONE,
        INIT,
        TRANSACTION_STARTING,
        TRANSACTION_STARTED,
        CASHIN_STARTING,
        CASHIN_STARTED,
        DROP_STARTING,
        DROP_STARTED,
        DROP_STOPPED,
        DROP_PAUSING,
        DROP_PAUSED,
        DROP_ENDED,
        DROP_ESCROW_REJECTING,
        DROP_ESCROW_REJECTED,
        DROP_ESCROW_ACCEPTING,
        DROP_ESCROW_ACCEPTED,
        DROP_ESCROW_DONE,
        CASHIN_ENDING,
        CASHIN_ENDED,
        DISPENSE_STARTING,
        DISPENSE_STARTED,
        DISPENSE_ENDING,
        DISPENSE_ENDED,
        TRANSACTION_ENDING,
        TRANSACTION_ENDED,
        OUT_OF_ORDER,
        ESCROWJAM_START,
        ESCROWJAM_OPEN_REQUEST,
        ESCROWJAM_CLEAR_WAIT,
        ESCROWJAM_END_REQUEST,
        ESCROWJAM_END,
        NOTEJAM_CLEAR_WAIT,
        NOTEJAM_START,
        NOTEJAM_END_REQUEST,
    }
}
