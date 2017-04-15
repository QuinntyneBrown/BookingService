import { Resource } from "./resource.model";

export const resourceActions = {
    ADD: "[Resource] Add",
    EDIT: "[Resource] Edit",
    DELETE: "[Resource] Delete",
    RESOURCES_CHANGED: "[Resource] Resources Changed"
};

export class ResourceEvent extends CustomEvent {
    constructor(eventName:string, resource: Resource) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { resource }
        });
    }
}

export class ResourceAdd extends ResourceEvent {
    constructor(resource: Resource) {
        super(resourceActions.ADD, resource);        
    }
}

export class ResourceEdit extends ResourceEvent {
    constructor(resource: Resource) {
        super(resourceActions.EDIT, resource);
    }
}

export class ResourceDelete extends ResourceEvent {
    constructor(resource: Resource) {
        super(resourceActions.DELETE, resource);
    }
}

export class ResourcesChanged extends CustomEvent {
    constructor(resources: Array<Resource>) {
        super(resourceActions.RESOURCES_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { resources }
        });
    }
}
