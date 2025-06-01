import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Asset } from '../assets.models';

@Component({
    selector: 'app-edit-asset',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule, MatProgressSpinnerModule],
    templateUrl: './edit-asset.component.html',
    styleUrls: ['./edit-asset.component.css']
})
export class EditAssetComponent implements OnInit {
    assetId!: string;
    asset: Asset = {
        id: 0,
        name: '',
        type: ''
    };
    loading: boolean = false;

    private route = inject(ActivatedRoute);
    private http = inject(HttpClient);
    private router = inject(Router);

    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('assetId');

        if (idParam) {
            this.assetId = idParam;
            this.loading = true;
            this.http.get<Asset>(`/api/asset/${this.assetId}`).subscribe({
                next: data => {
                    this.asset = data;
                    this.loading = false;
                },
                error: err => {
                    console.error('Failed to load asset', err);
                    this.loading = false;
                }
            });
        }
    }

    saveAsset() {
        const request = this.assetId
            ? this.http.put(`/api/asset/${this.assetId}`, this.asset)
            : this.http.post(`/api/asset`, this.asset);

        request.subscribe({
            next: () => {
                alert(`Asset ${this.assetId ? 'updated' : 'created'} successfully!`);
                this.router.navigate(['/assets']);
            },
            error: err => {
                console.error('Failed to save asset', err);
            }
        });
    }

    cancel() {
        this.router.navigate(['/assets']);
    }
}
