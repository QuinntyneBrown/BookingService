export class Booking { 
    public id:any;
    public name:string;

    public fromJSON(data: { name:string }): Booking {
        let booking = new Booking();
        booking.name = data.name;
        return booking;
    }
}
