import { BookingAdd, BookingDelete, BookingEdit, bookingActions } from "./booking.actions";
import { Booking } from "./booking.model";
import { BookingService } from "./booking.service";

const template = require("./booking-master-detail.component.html");
const styles = require("./booking-master-detail.component.scss");

export class BookingMasterDetailComponent extends HTMLElement {
    constructor(
        private _bookingService: BookingService = BookingService.Instance	
	) {
        super();
        this.onBookingAdd = this.onBookingAdd.bind(this);
        this.onBookingEdit = this.onBookingEdit.bind(this);
        this.onBookingDelete = this.onBookingDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "bookings"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.bookings = await this._bookingService.get();
        this.bookingListElement.setAttribute("bookings", JSON.stringify(this.bookings));
    }

    private _setEventListeners() {
        this.addEventListener(bookingActions.ADD, this.onBookingAdd);
        this.addEventListener(bookingActions.EDIT, this.onBookingEdit);
        this.addEventListener(bookingActions.DELETE, this.onBookingDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(bookingActions.ADD, this.onBookingAdd);
        this.removeEventListener(bookingActions.EDIT, this.onBookingEdit);
        this.removeEventListener(bookingActions.DELETE, this.onBookingDelete);
    }

    public async onBookingAdd(e) {

        await this._bookingService.add(e.detail.booking);
        this.bookings = await this._bookingService.get();
        
        this.bookingListElement.setAttribute("bookings", JSON.stringify(this.bookings));
        this.bookingEditElement.setAttribute("booking", JSON.stringify(new Booking()));
    }

    public onBookingEdit(e) {
        this.bookingEditElement.setAttribute("booking", JSON.stringify(e.detail.booking));
    }

    public async onBookingDelete(e) {

        await this._bookingService.remove(e.detail.booking.id);
        this.bookings = await this._bookingService.get();
        
        this.bookingListElement.setAttribute("bookings", JSON.stringify(this.bookings));
        this.bookingEditElement.setAttribute("booking", JSON.stringify(new Booking()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "bookings":
                this.bookings = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<Booking> { return this.bookings; }

    private bookings: Array<Booking> = [];
    public booking: Booking = <Booking>{};
    public get bookingEditElement(): HTMLElement { return this.querySelector("ce-booking-edit-embed") as HTMLElement; }
    public get bookingListElement(): HTMLElement { return this.querySelector("ce-booking-list-embed") as HTMLElement; }
}

customElements.define(`ce-booking-master-detail`,BookingMasterDetailComponent);
