import { Http, Headers, RequestOptions } from '@angular/http';
import { Component, OnInit } from '@angular/core';
import { MessageModel } from '../../models/message.model';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  private messages: MessageModel[];
  private messContent: string;

  constructor(
    private http: Http
  ) { }

  public ngOnInit() {
    this.loadData();
  }

  private loadData() {
    const url = 'http://localhost:56604/messages';
    // const url = 'http://samplequeuestorageapi.azurewebsites.net/messages';

    this.http.get(url)
      .subscribe(data => {
        if (data) {
          this.messages = data.json();
        }
      });
  }

  private onSend() {
    const message: MessageModel = {
      QueueMessage: this.messContent
    };

    const url = 'http://localhost:56604/messages';

    const headers = new Headers({ 'Content-Type': 'application/json' });
    const options = new RequestOptions({ headers: headers });

    this.http.put(url, JSON.stringify(message), options).subscribe(data => {});

    this.loadData();
  }
}
