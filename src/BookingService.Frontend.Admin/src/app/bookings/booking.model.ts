export class Booking { 
    public id: any;
    public resourceId: any;
    public name: string;
    public start: any;
    public end: any;
    public description: any;
    public isCancelled: any;

    public fromJSON(data: any): Booking {
        let booking = new Booking();

        booking.name = data.name;

        booking.resourceId = data.resourceId;

        booking.name = data.name;

        booking.start = data.start;

        booking.end = data.end;

        booking.description = data.description;

        booking.isCancelled = data.isCancelled;

        return booking;
    }
}
