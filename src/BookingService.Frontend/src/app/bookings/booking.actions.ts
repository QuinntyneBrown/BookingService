import { Booking } from "./booking.model";

export const bookingActions = {
    ADD: "[Booking] Add",
    EDIT: "[Booking] Edit",
    DELETE: "[Booking] Delete",
    BOOKINGS_CHANGED: "[Booking] Bookings Changed"
};

export class BookingEvent extends CustomEvent {
    constructor(eventName:string, booking: Booking) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { booking }
        });
    }
}

export class BookingAdd extends BookingEvent {
    constructor(booking: Booking) {
        super(bookingActions.ADD, booking);        
    }
}

export class BookingEdit extends BookingEvent {
    constructor(booking: Booking) {
        super(bookingActions.EDIT, booking);
    }
}

export class BookingDelete extends BookingEvent {
    constructor(booking: Booking) {
        super(bookingActions.DELETE, booking);
    }
}

export class BookingsChanged extends CustomEvent {
    constructor(bookings: Array<Booking>) {
        super(bookingActions.BOOKINGS_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { bookings }
        });
    }
}
