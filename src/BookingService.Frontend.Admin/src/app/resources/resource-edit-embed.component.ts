import { Resource } from "./resource.model";
import { EditorComponent } from "../shared";
import { ResourceDelete, ResourceEdit, ResourceAdd } from "./resource.actions";

const template = require("./resource-edit-embed.component.html");
const styles = require("./resource-edit-embed.component.scss");

export class ResourceEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onCreate = this.onCreate.bind(this);
    }

    static get observedAttributes() {
        return [
            "resource",
            "resource-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.resource ? "Edit Resource": "Create Resource";

        if (this.resource) {                
            this._nameInputElement.value = this.resource.name;  
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

    public onSave() {
        const resource = {
            id: this.resource != null ? this.resource.id : null,
            name: this._nameInputElement.value
        } as Resource;
        
        this.dispatchEvent(new ResourceAdd(resource));            
    }

    public onCreate() {        
        this.dispatchEvent(new ResourceEdit(new Resource()));            
    }

    public onDelete() {        
        const resource = {
            id: this.resource != null ? this.resource.id : null,
            name: this._nameInputElement.value
        } as Resource;

        this.dispatchEvent(new ResourceDelete(resource));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "resource-id":
                this.resourceId = newValue;
                break;
            case "resource":
                this.resource = JSON.parse(newValue);
                if (this.parentNode) {
                    this.resourceId = this.resource.id;
                    this._nameInputElement.value = this.resource.name != undefined ? this.resource.name : "";
                    this._titleElement.textContent = this.resourceId ? "Edit Resource" : "Create Resource";
                }
                break;
        }           
    }

    public resourceId: any;
    
	public resource: Resource;
    
    private get _createButtonElement(): HTMLElement { return this.querySelector(".resource-create") as HTMLElement; }
    
	private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    
	private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    
	private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    
	private get _nameInputElement(): HTMLInputElement { return this.querySelector(".resource-name") as HTMLInputElement;}
}

customElements.define(`ce-resource-edit-embed`,ResourceEditEmbedComponent);
