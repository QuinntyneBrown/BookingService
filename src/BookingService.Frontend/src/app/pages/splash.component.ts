import { ApiService } from "../shared";
import { getCurrentPositionAsync } from "../utilities";

const template = require("./splash.component.html");
const styles = require("./splash.component.scss");

export class SplashComponent extends HTMLElement {
    constructor(
        private _apiService: ApiService = ApiService.Instance
    ) {
        super();
        this.onGetNearByEvents = this.onGetNearByEvents.bind(this);
    }
    
    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        const coordinates = await getCurrentPositionAsync();        
        const address = await this._apiService.getAddress({
            longitude: coordinates.longitude,
            latitude: coordinates.latitude
        });        
        this._addressInputElement.value = address;
    }

    public async onGetNearByEvents() {        
        var closeEvents = await this._apiService.getClosetEvents({
            address: this._addressInputElement.value
        });
        
        this._nearByEventsElement.setAttribute("events", JSON.stringify(closeEvents));
    }

    private _setEventListeners() {
        this._getNearByEventsButtonElement.addEventListener("click", this.onGetNearByEvents);
    }

    disconnectedCallback() {
        this._getNearByEventsButtonElement.removeEventListener("click", this.onGetNearByEvents);
    }

    private get _addressInputElement(): HTMLInputElement { return this.querySelector(".address") as HTMLInputElement; }

    private get _getNearByEventsButtonElement(): HTMLElement { return this.querySelector("ce-button") as HTMLElement; }

    private get _nearByEventsElement(): HTMLElement { return this.querySelector("ce-near-by-events") as HTMLElement; }

}

customElements.define(`ce-splash`,SplashComponent);