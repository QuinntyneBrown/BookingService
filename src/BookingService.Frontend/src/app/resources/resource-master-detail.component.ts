import { ResourceAdd, ResourceDelete, ResourceEdit, resourceActions } from "./resource.actions";
import { Resource } from "./resource.model";
import { ResourceService } from "./resource.service";

const template = require("./resource-master-detail.component.html");
const styles = require("./resource-master-detail.component.scss");

export class ResourceMasterDetailComponent extends HTMLElement {
    constructor(
        private _resourceService: ResourceService = ResourceService.Instance	
	) {
        super();
        this.onResourceAdd = this.onResourceAdd.bind(this);
        this.onResourceEdit = this.onResourceEdit.bind(this);
        this.onResourceDelete = this.onResourceDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "resources"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.resources = await this._resourceService.get();
        this.resourceListElement.setAttribute("resources", JSON.stringify(this.resources));
    }

    private _setEventListeners() {
        this.addEventListener(resourceActions.ADD, this.onResourceAdd);
        this.addEventListener(resourceActions.EDIT, this.onResourceEdit);
        this.addEventListener(resourceActions.DELETE, this.onResourceDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(resourceActions.ADD, this.onResourceAdd);
        this.removeEventListener(resourceActions.EDIT, this.onResourceEdit);
        this.removeEventListener(resourceActions.DELETE, this.onResourceDelete);
    }

    public async onResourceAdd(e) {

        await this._resourceService.add(e.detail.resource);
        this.resources = await this._resourceService.get();
        
        this.resourceListElement.setAttribute("resources", JSON.stringify(this.resources));
        this.resourceEditElement.setAttribute("resource", JSON.stringify(new Resource()));
    }

    public onResourceEdit(e) {
        this.resourceEditElement.setAttribute("resource", JSON.stringify(e.detail.resource));
    }

    public async onResourceDelete(e) {

        await this._resourceService.remove(e.detail.resource.id);
        this.resources = await this._resourceService.get();
        
        this.resourceListElement.setAttribute("resources", JSON.stringify(this.resources));
        this.resourceEditElement.setAttribute("resource", JSON.stringify(new Resource()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "resources":
                this.resources = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<Resource> { return this.resources; }

    private resources: Array<Resource> = [];
    public resource: Resource = <Resource>{};
    public get resourceEditElement(): HTMLElement { return this.querySelector("ce-resource-edit-embed") as HTMLElement; }
    public get resourceListElement(): HTMLElement { return this.querySelector("ce-resource-list-embed") as HTMLElement; }
}

customElements.define(`ce-resource-master-detail`,ResourceMasterDetailComponent);
