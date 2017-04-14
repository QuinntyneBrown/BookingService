import { Booking } from "./booking.model";

const template = require("./booking-list-embed.component.html");
const styles = require("./booking-list-embed.component.scss");

export class BookingListEmbedComponent extends HTMLElement {
    constructor(
        private _document: Document = document
    ) {
        super();
    }


    static get observedAttributes() {
        return [
            "bookings"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {        
        for (let i = 0; i < this.bookings.length; i++) {
            let el = this._document.createElement(`ce-booking-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.bookings[i]));
            this.appendChild(el);
        }    
    }

    bookings:Array<Booking> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "bookings":
                this.bookings = JSON.parse(newValue);
                if (this.parentElement)
                    this.connectedCallback();
                break;
        }
    }
}

customElements.define("ce-booking-list-embed", BookingListEmbedComponent);
