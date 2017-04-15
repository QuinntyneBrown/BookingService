import { Booking } from "./booking.model";
import { EditorComponent } from "../shared";
import { BookingDelete, BookingEdit, BookingAdd } from "./booking.actions";
import { ResourceService, Resource } from "../resources";

const template = require("./booking-edit-embed.component.html");
const styles = require("./booking-edit-embed.component.scss");

export class BookingEditEmbedComponent extends HTMLElement {
    constructor(
        private _resourceService: ResourceService = ResourceService.Instance
    ) {
        super();
        this.onCreate = this.onCreate.bind(this);
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

        this.resources = await this._resourceService.get();

        for (let i = 0; i < this.resources.length; i++) {
            let option = document.createElement("option");
            option.textContent = this.resources[i].name;
            option.value = this.resources[i].id;
            this._selectElement.appendChild(option);
        }

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
        this._createButtonElement.addEventListener("click", this.onCreate);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
        this._createButtonElement.removeEventListener("click", this.onCreate);
    }

    public onCreate() {
        this.dispatchEvent(new BookingEdit(new Booking()));
    }

    public onSave() {
        const booking = {
            id: this.booking != null ? this.booking.id : null,
            name: this._nameInputElement.value,
            start: this._startInputElement.value,
            end: this._endInputElement.value,
            isCancelled: this._isCancelledInputElement.value
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

    public resources: Array<Resource> = [];

    private get _selectElement(): HTMLSelectElement { return this.querySelector("select") as HTMLSelectElement; }

    private get _createButtonElement(): HTMLElement { return this.querySelector(".booking-create") as HTMLElement; }
    
    private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }

    private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };

    private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };

    private get _nameInputElement(): HTMLInputElement { return this.querySelector(".booking-name") as HTMLInputElement; }

    private get _startInputElement(): HTMLInputElement { return this.querySelector(".booking-start") as HTMLInputElement; }

    private get _endInputElement(): HTMLInputElement { return this.querySelector(".booking-end") as HTMLInputElement; }

    private get _descriptionElement(): HTMLInputElement { return this.querySelector(".booking-description") as HTMLInputElement; }

    private get _isCancelledInputElement(): HTMLInputElement { return this.querySelector(".booking-cancelled") as HTMLInputElement; }
}

customElements.define(`ce-booking-edit-embed`,BookingEditEmbedComponent);
