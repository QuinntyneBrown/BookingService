import { createElement } from "../utilities";

const template = require("./near-by-events.component.html");
const styles = require("./near-by-events.component.scss");

export class NearByEventsComponent extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes () {
        return [
            "events"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.nearByEventList.innerHTML = "";
        for (var i = 0; i < this.events.length; i++) {
            var nearByEventElement = createElement("<ce-near-by-event></ce-near-by-event>");
            nearByEventElement.setAttribute("event", JSON.stringify(this.events[i]));
            this.nearByEventList.appendChild(nearByEventElement);
        }
    }

    private _setEventListeners() {

    }

    disconnectedCallback() {

    }

    private events: Array<any> = [];

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "events":
                this.events = JSON.parse(newValue);

                if (this.parentNode)
                    this._bind();

                break;
        }
    }

    private get nearByEventList(): HTMLElement { return this.querySelector(".near-by-event-list") as HTMLElement; }
}

customElements.define(`ce-near-by-events`,NearByEventsComponent);
