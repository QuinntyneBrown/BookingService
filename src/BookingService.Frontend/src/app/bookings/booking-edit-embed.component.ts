import { Booking } from "./booking.model";
import { EditorComponent } from "../shared";
import {  BookingDelete, BookingEdit, BookingAdd } from "./booking.actions";

const template = require("./booking-edit-embed.component.html");
const styles = require("./booking-edit-embed.component.scss");

export class BookingEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
    }

    static get observedAttributes() {
        return [
            "booking",
            "booking-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.booking ? "Edit Booking": "Create Booking";

        if (this.booking) {                
            this._nameInputElement.value = this.booking.name;  
        } else {
            this._deleteButtonElement.style.display = "none";
        }     
    }

    private _setEventListeners() {
        this._saveButtonElement.addEventListener("click", this.onSave);
        this._deleteButtonElement.addEventListener("click", this.onDelete);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
    }

    public onSave() {
        const booking = {
            id: this.booking != null ? this.booking.id : null,
            name: this._nameInputElement.value
        } as Booking;
        
        this.dispatchEvent(new BookingAdd(booking));            
    }

    public onDelete() {        
        const booking = {
            id: this.booking != null ? this.booking.id : null,
            name: this._nameInputElement.value
        } as Booking;

        this.dispatchEvent(new BookingDelete(booking));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "booking-id":
                this.bookingId = newValue;
                break;
            case "booking":
                this.booking = JSON.parse(newValue);
                if (this.parentNode) {
                    this.bookingId = this.booking.id;
                    this._nameInputElement.value = this.booking.name != undefined ? this.booking.name : "";
                    this._titleElement.textContent = this.bookingId ? "Edit Booking" : "Create Booking";
                }
                break;
        }           
    }

    public bookingId: any;
    public booking: Booking;
    
    private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    private get _nameInputElement(): HTMLInputElement { return this.querySelector(".booking-name") as HTMLInputElement;}
}

customElements.define(`ce-booking-edit-embed`,BookingEditEmbedComponent);
