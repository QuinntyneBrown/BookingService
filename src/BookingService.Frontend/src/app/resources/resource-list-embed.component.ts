import { Resource } from "./resource.model";

const template = require("./resource-list-embed.component.html");
const styles = require("./resource-list-embed.component.scss");

export class ResourceListEmbedComponent extends HTMLElement {
    constructor(
        private _document: Document = document
    ) {
        super();
    }


    static get observedAttributes() {
        return [
            "resources"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {        
        for (let i = 0; i < this.resources.length; i++) {
            let el = this._document.createElement(`ce-resource-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.resources[i]));
            this.appendChild(el);
        }    
    }

    resources:Array<Resource> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "resources":
                this.resources = JSON.parse(newValue);
                if (this.parentElement)
                    this.connectedCallback();
                break;
        }
    }
}

customElements.define("ce-resource-list-embed", ResourceListEmbedComponent);
