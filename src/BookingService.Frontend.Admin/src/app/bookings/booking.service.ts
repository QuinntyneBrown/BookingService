import { fetch } from "../utilities";
import { Booking } from "./booking.model";

export class BookingService {
    constructor(private _fetch = fetch) { }

    private static _instance: BookingService;

    public static get Instance() {
        this._instance = this._instance || new BookingService();
        return this._instance;
    }

    public get(): Promise<Array<Booking>> {
        return this._fetch({ url: "/api/booking/get", authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { bookings: Array<Booking> }).bookings;
        });
    }

    public getById(id): Promise<Booking> {
        return this._fetch({ url: `/api/booking/getbyid?id=${id}`, authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { booking: Booking }).booking;
        });
    }

    public add(booking) {
        return this._fetch({ url: `/api/booking/add`, method: "POST", data: { booking }, authRequired: true  });
    }

    public remove(options: { id : number }) {
        return this._fetch({ url: `/api/booking/remove?id=${options.id}`, method: "DELETE", authRequired: true  });
    }
    
}
