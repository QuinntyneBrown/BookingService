import { jsonp } from "./jsonp";

export function getIPInfoAsync(): Promise<any> {
    return jsonp("https://ipinfo.io/json");
}