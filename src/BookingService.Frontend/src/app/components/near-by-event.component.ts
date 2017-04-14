const template = require("./near-by-event.component.html");
const styles = require("./near-by-event.component.scss");

export class NearByEventComponent extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes () {
        return [
            "event"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        if(this.event)
            this._title.innerHTML = `${this.event.name} ${this.event.eventLocation.address}`;
    }

    private _setEventListeners() {

    }

    disconnectedCallback() {

    }

    private event: any;

    private get _title(): HTMLElement { return this.querySelector("h4") as HTMLElement; }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "event":
                this.event = JSON.parse(newValue);
                
                if (this.parentNode)
                    this._bind();

                break;
        }
    }
}

customElements.define(`ce-near-by-event`,NearByEventComponent);
