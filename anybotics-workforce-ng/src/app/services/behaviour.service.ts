import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BehaviourSnippet } from "../models/behaviour-dto.model";
import { HttpClient } from "@angular/common/http";
import { ConfigService } from "./config.service";

@Injectable({
    providedIn: 'root'
})
export class BehaviourService {
    private apiUrl = this.configService.config.behavioursApiUrl;

    constructor(private configService: ConfigService, private http: HttpClient) { }

    // Method to get all behaviour snippets
    getBehaviours(): Observable<BehaviourSnippet[]> {
        return this.http.get<BehaviourSnippet[]>(this.apiUrl);
    }
}